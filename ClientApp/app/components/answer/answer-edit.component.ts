import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Component({
    selector: "answer-edit",
    templateUrl: "./answer-edit.component.html",
    styleUrls: ["./answer-edit.component.css"]
})
export class AnswerEditComponent {
    title: string;
    answer: Answer;
    form: FormGroup;

    //this will be true when editing an existing answer and false when 
    //creating anew one
    editMode: boolean;

    constructor(private activatedRoute: ActivatedRoute,
        private http: HttpClient,
        private router: Router,
        private fb: FormBuilder,
        @Inject('BASE_URL') private baseUrl: string) 
    {
        this.createForm();
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

    createForm(){
        this.form = this.fb.group({
            'Text': ['',Validators.required],
            'Value': ['',[
                Validators.required,
                Validators.min(-5),
                Validators.max(5)
            ]]
        });
    }
    onSubmit(answer:Answer){
        var tempAnswer = <Answer>{};
        tempAnswer.Value = this.form.value.Value;
        tempAnswer.QuestionId = this.answer.QuestionId;
        tempAnswer.Text = this.form.value.Text;

        var url = this.baseUrl + "api/answer";

        if(this.editMode){
            tempAnswer.Id = this.answer.Id;
            this.http.post<Answer>(url,tempAnswer).subscribe(res => {
                var v = res;
                console.log("Answer " + v.Id + "has been updated");
                this.router.navigate(["question/edit",v.QuestionId]);
            },error => console.error(error))
        }
        else{
            this.http.put<Answer>(url,tempAnswer).subscribe(res => {
                var v = res;
                this.router.navigate(["question/edit",v.QuestionId])
            },error => console.error(error))
        }
    }

    onBack(){
        this.router.navigate(["question/edit",this.answer.QuestionId]);
    }

    getFormControl(name:string){
        return this.form.get(name);
    }

    //returns true of the form control is valid
    isValid(name:string){
        var a = this.getFormControl(name);
        return a && a.valid;
    }

    //returns true of the form control as been changed
    isChanged(name:string){
        var e = this.form.get(name);
        return e && (e.dirty || e.touched);
    }

    //returns true if the FormControl is invalid aftter user has made changes
    hasErrors(name:string){
        var e = this.form.get(name);
        return e && (e.dirty || e.touched) && !e.valid;
    }
}