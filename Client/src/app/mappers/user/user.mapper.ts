import { UserDTO } from "../../dtos/user/user.dto";
import { User } from "../../entities/user/user.entity";
import { Mapper } from "../base.mapper";

export class UserMapper extends Mapper<UserDTO, User> {
    override map(param: UserDTO): User {
        return {
            id: param.id,
            userName: param.username,
            fullName: param.fullName,
            roles: param.roles,
            profileImage: param.profileImage
        }
    }
}



