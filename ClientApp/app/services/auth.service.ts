import { EventEmitter, Injectable, Inject, PLATFORM_ID } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import 'rxjs/Rx';


@Injectable()
export class AuthService{
    authKey: string = "auth";
    clientId: string = "TestMaker";

    constructor(private http:HttpClient, @Inject(PLATFORM_ID) private paltformId: any){
    }

    //performs the login
    login(username: string, password: string): Observable<boolean>{
        var url = "api/auth/jwt";
        var data = {
            username: username,
            password: password,
            client_id: this.clientId,
            //required when signing up with username/password
            grant_type: "password",
            //space-seperated list of all the scopes for which the token is issued
            scope: "offline_access profile email"
        };

        return this.http.post<TokenResponse>(url,data).map(
            (res)=> {
                let token = res && res.token;
                //if the token exists, login has been successful
                if(token){
                    //store username and jwt token
                    this.setAuth(res);
                    //successful login
                    return true;
                }

                //failed login
                return Observable.throw('Unauthorized');
            })
            .catch(error => {
                return new Observable<any>(error);
            });
    }
}