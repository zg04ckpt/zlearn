import { UserDTO } from "../dtos/user.dto";
import { User } from "../entities/user.entity";
import { Mapper } from "./base.mapper";

export class UserMapper extends Mapper<UserDTO, User> {
    override map(param: UserDTO): User {
        return {
            id: param.id,
            userName: param.userName,
            roles: param.roles
        }
    }
}



