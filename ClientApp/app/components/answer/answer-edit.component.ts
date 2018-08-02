import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
    selector: "answer-edit",
    templateUrl: "./answer-edit.component.html",
    styleUrls: ["./answer-edit.component.css"]
})
export class AnswerEditComponent {
    title: string;
    answer: Answer;

    //this will be true when editing an existing answer and false when 
    //creating anew one
    editMode: boolean;

    constructor(private activatedRoute: ActivatedRoute,
        private http: HttpClient,
        private router: Router,
        @Inject('BASE_URL') private baseUrl: string) 
    {
        //create an empty object from the Answer interface
        this.answer = <Answer>{};

        var id = +this.activatedRoute.snapshot.params['id'];
        //check if we are in edit mode or not
        this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");

        if (this.editMode) {
            //fetch the question from the server
            var url = this.baseUrl + "api/answer/" + id;
            this.http.get<Answer>(url).subscribe(q => {
                this.answer = q;
                this.title = "Edit - " + this.answer.Text;
            }, error => console.error(error)
            );
        }
        else {
            this.answer.QuestionId = id;
            this.title = "Create a new answer"
        }
    }

    onSubmit(answer:Answer){
        var url = this.baseUrl + "api/answer";

        if(this.editMode){
            this.http.post<Answer>(url,answer).subscribe(res => {
                var v = res;
                console.log("Answer " + v.Id + "has been updated");
                this.router.navigate(["question/edit",v.QuestionId]);
            },error => console.error(error))
        }
        else{
            this.http.put<Answer>(url,answer).subscribe(res => {
                var v = res;
                this.router.navigate(["question/edit",v.QuestionId])
            },error => console.error(error))
        }
    }

    onBack(){
        this.router.navigate(["question/edit",this.answer.QuestionId]);
    }
}