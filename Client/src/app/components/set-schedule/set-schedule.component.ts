import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Subject } from 'src/app/models/schedule/subject';
import { Week } from 'src/app/models/schedule/week';
import { Time } from "@angular/common"
import { WeekTable } from 'src/app/models/schedule/week-table';
import { Session } from 'src/app/models/schedule/session';
import * as XLSX from 'xlsx';
import { NavigationStart, Router } from '@angular/router';

@Component({
  selector: 'app-set-schedule',
  templateUrl: './set-schedule.component.html',
  styleUrls: ['./set-schedule.component.css']
})
export class SetScheduleComponent {
  constructor(private httpClient : HttpClient,
    private router: Router
  ) {}

  showListSubjects: Subject[] = [];
  listSubjects: Subject[] = [];
  listWeek: Week[] = [];
  mp: Map<string,string> = new Map();

  filter: string[] = [
    "",
    "D20",
    "D21",
    "D22",
    "D23",
    "E20",
    "E21",
    "E22",
    "E23"
  ];
  curSelection: string = "";

  currentWeek = 0;
  table: WeekTable[] = [];

  succeed: {
    id: String,
    subName: String,
    group: String,
    th: String,
    st: number,
    room: String,
    teacher: String
  }[] = [];

  onSelectChange(event: any) {
    this.curSelection = event.target.value;
    this.showListSubjects = this.listSubjects.filter(item => item.lop.includes(this.curSelection));
    this.showListSubjects.forEach(item => {
      item.isDup = this.check(item);
    });
  }

  load(value: number)
  {
    this.currentWeek += value;
    if(this.currentWeek < 0) this.currentWeek = this.listWeek.length - 1;
    if(this.currentWeek >= this.listWeek.length) this.currentWeek = 0;
  }

  find(key : string)
  {
    key = key.toLowerCase();
    this.showListSubjects = [];
    this.listSubjects.forEach(item => {
      console.log(item.ten_mon);
      
      if (item.lop.toLowerCase().includes(key) || 
      item.ten_mon.toLowerCase().includes(key) ||
      item.tkb.toLowerCase().includes(key))
      {
        item.isDup = this.check(item);
        this.showListSubjects.push(item);
      }
    });
    this.showListSubjects = this.showListSubjects.filter(item => item.lop.includes(this.curSelection));
  }

  ngOnInit() {
    this.httpClient.get<any>('assets/schedule.json').subscribe(content => {
      const arr = content.data.ds_nhom_to as any[];
      const arr2 = content.data.ds_mon_hoc as any[];
      arr2.forEach(item => {
        this.mp.set(item.ma, item.ten);
      });

      arr.forEach(item => {
        this.listSubjects.push({
          ma_mon: item.ma_mon,
          ten_mon: this.mp.get(item.ma_mon)!,
          so_tc: item.so_tc,
          nhom_to: item.nhom_to,
          lop: item.lop,
          to_th: item.to,
          tkb: item.tkb,
          isDup: 0
        });
      });
    });

    this.httpClient.get<any>('assets/week.json').subscribe(content => {
      const arr = content.data.ds_tuan_tkb as any[];
      
      arr.forEach(item => {
        const arr = item.ngay_bat_dau.split('/');
        item.ngay_bat_dau = new Date(arr[2], arr[1] - 1, arr[0]);
        const arr2 = item.ngay_ket_thuc.split('/');
        item.ngay_ket_thuc = new Date(arr2[2], arr2[1] - 1, arr2[0]);
      });
      
      arr.forEach(item => {
        this.listWeek.push({
          name: item.tuan_hoc_ky.toString(),
          start: item.ngay_bat_dau,
          end: item.ngay_ket_thuc,
          t2: [],
          t3: [],
          t4: [],
          t5: [],
          t6: [],
          t7: [],
          cn: []
        });

        this.table.push({
          check: Array(15).fill(null).map(() => Array(7).fill("")),
          span: Array(15).fill(null).map(() => Array(7).fill(1))
        });
      });
    });
  }

  ngOnDestroy() {
    if(this.succeed.length > 0)
    {
      if(confirm("Lịch được chọn sẽ mất khi chuyển hướng, bạn có muốn lưu lịch hiện tại không?"))
      {
        this.saveDataAsExcel();
      }
    }
  }
/*
Thứ 6,từ 07:00 đến 08:50,Ph 401-A2,GV V.T.Tú,16/08/24 đến 08/11/24\n
Thứ 6,từ 09:00 đến 09:50,Ph 401-A2,GV V.T.Tú,11/10/24 đến 01/11/24
*/

  add(sub: Subject)
  {
    let teacherName: string = "";
    let roo: string = "";
    if(sub.tkb === "") return;
    const w = sub.tkb.split("<hr>");
    w.forEach(item => {
      const inf = item.split(",");
      if(inf.length < 5) 
      {
        inf.splice(2, 0, "");
        inf.splice(3, 0, "");
      }
      //get week time
      const start = inf[4].substring(0, 8).split("/") as any[];
      start[2] = "20"+start[2];
      const startDate = new Date(start[2]+"/"+start[1]+"/"+start[0]);
      let endDate = startDate;
      if(inf[4].length >= 21)
      {
        const end = inf[4].substring(13, 21).split("/") as any[];
        end[2] = "20"+end[2];
        endDate = new Date(end[2]+"/"+end[1]+"/"+end[0]);
      }
    
      // console.log(startDate.toLocaleDateString() + "-" + endDate.toLocaleDateString());
      

      var day:number;
      if(inf[0] == "Thứ 2") day = 0;
      else if(inf[0] == "Thứ 3") day = 1;
      else if(inf[0] == "Thứ 4") day = 2;
      else if(inf[0] == "Thứ 5") day = 3;
      else if(inf[0] == "Thứ 6") day = 4;
      else if(inf[0] == "Thứ 7") day = 5;
      else day = 6;
      const startSe = inf[1].substring(3, 8);
      const endSe = inf[1].substring(13, 18);
      // console.log(startSe + "-" + startDate.toLocaleDateString() + '-' + endDate.toLocaleDateString());
      
      let k = 0;
      this.listWeek.forEach(week => {
      console.log(week.start.toLocaleDateString() + '-' + startDate.toLocaleDateString() + '-' + endDate.toLocaleDateString() + '-' + week.end.toLocaleDateString() + '-' + (week.start <= startDate && endDate <= week.end));

        if(week.start >= startDate && week.start < endDate)
        {
          // console.log(startDate.toLocaleDateString() + "-" + endDate.toLocaleDateString());
          
          // console.log(startSe + "-" + startDate.toLocaleDateString() + '-' + endDate.toLocaleDateString());
          
          teacherName = inf[3];
          roo = inf[2];

          const session: Session = {
            start: { hours: +startSe.split(":")[0], minutes: +startSe.split(":")[1]}, 
            end: { hours: +endSe.split(":")[0], minutes: +endSe.split(":")[1]},
            nameSub: sub.ten_mon,
            room: inf[2],
            teacher: inf[3],
            group: sub.nhom_to
          }
          
          for(let hour=session.start.hours; hour<=session.end.hours; ++hour)
          {
            // console.log(hour + '-' + day);
            
            this.table[k].check[hour-7][day] = session.nameSub;
          }
        }
        k++;
      });
    });

    //gộp ô -> duyệt qua k tuần
    for(let k=0; k<this.listWeek.length; k++)
    {
      for(let j=0; j<7; j++)
      {
        for(let i=0; i<15; i++)
        {
          if(this.table[k].check[i][j] != "")
          {
            let h = i+1;
            while(h < 15 && this.table[k].check[h][j] == this.table[k].check[i][j])
            {
              this.table[k].span[i][j]++;
              this.table[k].span[h][j] = 0;
              this.table[k].check[h][j] = "";
              h++;
            }
          }
        }
      }
    }
    
    this.succeed.push({
      id: sub.ma_mon,
      subName: sub.ten_mon,
      group: sub.nhom_to,
      th: sub.to_th,
      st: Number(sub.so_tc),
      room: roo,
      teacher: teacherName,
    })

    this.showListSubjects.forEach(item => {
      item.isDup = this.check(item);
    });
  }

  remove(subName: String)
  {
    //xóa ô -> duyệt qua k tuần
    for(let k=0; k<this.listWeek.length; k++)
    {
      for(let j=0; j<7; j++)
      {
        for(let i=0; i<15; i++)
        {
          if(this.table[k].check[i][j] == subName)
          {
            let loop = this.table[k].span[i][j];
            for(let h=0; h<loop; ++h)
            {
              this.table[k].check[i][j] = "";
              this.table[k].span[i][j] = 1;
            }
          }
        }
      }
    }
    this.succeed = this.succeed.filter(item => item.subName !== subName);
  }

  check(sub: Subject) : number {
    //check môn đã đăng kí
    for(let i=0; i<this.succeed.length; i++)
    {
      if(this.succeed[i].id === sub.ma_mon) return 2;
    }

    let valid = 0;
    if(sub.tkb === "") return 0;
    const w = sub.tkb.split("<hr>");
    w.forEach(item => {
      const inf = item.split(",");
      if(inf.length < 5) 
      {
        inf.splice(2, 0, "");
        inf.splice(3, 0, "");
      }
      //Thứ 7,từ 14:00 đến 15:50,17/08/24 
      //get week time
      const start = inf[4].substring(0, 8).split("/") as any[];
      start[2] = "20"+start[2];
      const startDate = new Date(start[2]+"/"+start[1]+"/"+start[0]);
      let endDate: Date;
      if(inf[4].length >= 21)
      {
        const end = inf[4].substring(13, 21).split("/") as any[];
        end[2] = "20"+end[2];
        endDate = new Date(end[2]+"/"+end[1]+"/"+end[0]);
      }
      else
        endDate = startDate

      let day:number = 0;
      if(inf[0] === "Thứ 2") day = 0;
      else if(inf[0] === "Thứ 3") day = 1;
      else if(inf[0] === "Thứ 4") day = 2;
      else if(inf[0] === "Thứ 5") day = 3;
      else if(inf[0] === "Thứ 6") day = 4;
      else if(inf[0] === "Thứ 7") day = 5;
      else day = 6;
      const startSe = inf[1].substring(3, 8);
      const endSe = inf[1].substring(13, 18);
      
      let k = 0;
      this.listWeek.forEach(week => {
        if(week.start >= startDate && week.end <= endDate)
        {
          const session: Session = {
            start: { hours: +startSe.split(":")[0], minutes: +startSe.split(":")[1]}, 
            end: { hours: +endSe.split(":")[0], minutes: +endSe.split(":")[1]},
            nameSub: "",
            room: inf[2],
            teacher: inf[3],
            group: ""
          }

          for(let hour=session.start.hours; hour<=session.end.hours; ++hour)
          {
            if(this.table[k].check[hour-7][day] != "") 
            {
              valid = 1;
              return;
            }
          }
        }
        k++;
      });

      if(valid == 1) return;
    });
    return valid;
  }

  saveDataAsExcel() {
    const result: {
      ten_mon_hoc: String,
      nhom: String,
      to_th: String,
      so_tin_chi: number,
      phong: String,
      ten_giang_vien: String
    } [] = [];
    this.succeed.forEach(item => {
      result.push({
        ten_mon_hoc: item.subName,
        nhom: item.group,
        to_th: item.th,
        so_tin_chi: item.st,
        phong: item.room,
        ten_giang_vien: item.teacher
      })
    })
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(result);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, `tkb-zg04.xlsx`);
  }
}
