import { environment } from "../../../environments/environment";
import { UserDetailDTO } from "../../dtos/user/user-detail.dto";
import { UserDetail } from "../../entities/user/user-detail.entity";
import { Gender } from "../../enums/gender.enum";
import { Mapper } from "../base.mapper";

export class UserDetailMapper extends Mapper<UserDetailDTO, UserDetail> {
    override map(param: UserDetailDTO): UserDetail {
        var list: {name: string, url: string}[] = [];
        if(param.socialLinks != "")
        {
            param.socialLinks?.split('|').forEach(e => {
                list.push({
                    name: e.split(",")[0],
                    url: e.split(",")[1],
                });
            });
        }
        
        return {
            firstName: param.firstName,
            lastName: param.lastName,
            email: param.email,
            phoneNum: param.phoneNum,
            gender: param.gender as Gender,
            dayOfBirth: param.dayOfBirth,
            address: param.address,
            intro: param.intro,
            socialLinks: list,
            image: null,
            imageUrl: param.imageUrl? environment.baseUrl + param.imageUrl : null
        }
    }
}