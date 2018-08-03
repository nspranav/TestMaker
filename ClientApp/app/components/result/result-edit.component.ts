import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
    selector: "result-edit",
    templateUrl: "./result-edit.component.html",
    styleUrls: ["./result-edit.component.css"]
})
export class ResultEditComponent {
    title: string;
    result: Result;

    //this will be true when editing an existing result and false when 
    //creating anew one
    editMode: boolean;

    constructor(private activatedRoute: ActivatedRoute,
        private http: HttpClient,
        private router: Router,
        @Inject('BASE_URL') private baseUrl: string) 
    {
        //create an empty object from the Result interface
        this.result = <Result>{};

        var id = +this.activatedRoute.snapshot.params['id'];
        //check if we are in edit mode or not
        this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");

        if (this.editMode) {
            //fetch the quiz from the server
            var url = this.baseUrl + "api/result/" + id;
            this.http.get<Result>(url).subscribe(q => {
                this.result = q;
                this.title = "Edit - " + this.result.Text;
            }, error => console.error(error)
            );
        }
        else {
            this.result.QuizId = id;
            this.title = "Create a new result"
        }
    }

    onSubmit(result:Result){
        var url = this.baseUrl + "api/result";

        if(this.editMode){
            this.http.post<Result>(url,result).subscribe(res => {
                var v = res;
                console.log("Result " + v.Id + "has been updated");
                this.router.navigate(["quiz/edit",v.QuizId]);
            },error => console.error(error))
        }
        else{
            this.http.put<Result>(url,result).subscribe(res => {
                var v = res;
                this.router.navigate(["quiz/edit",v.QuizId])
            },error => console.error(error))
        }
    }

    onBack(){
        this.router.navigate(["quiz/edit",this.result.QuizId]);
    }
}