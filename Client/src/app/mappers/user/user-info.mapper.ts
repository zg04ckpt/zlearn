import { environment } from "../../../environments/environment";
import { UserDetailDTO } from "../../dtos/user/user-detail.dto";
import { UserInfoDTO } from "../../dtos/user/user-info.dto";
import { UserDetail } from "../../entities/user/user-detail.entity";
import { UserInfo } from "../../entities/user/user-info.entity";
import { Gender } from "../../enums/gender.enum";
import { Mapper } from "../base.mapper";

export class UserInfoMapper extends Mapper<UserInfoDTO, UserInfo> {
    override map(param: UserInfoDTO): UserInfo {
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
            id: param.id,
            firstName: param.firstName,
            lastName: param.lastName,
            username: param.username,
            email: param.email,
            phoneNum: param.phoneNum,
            gender: param.gender as Gender,
            dayOfBirth: param.dayOfBirth,
            address: param.address,
            intro: param.intro,
            socialLinks: list,
            imageUrl: param.imageUrl? environment.baseUrl + param.imageUrl : null,
            likes: param.likes,
            isLiked: param.isLiked
        }
    }
}