import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Component({
    selector: "question-edit",
    templateUrl: "./question-edit.component.html",
    styleUrls: ["./question-edit.component.css"]
})
export class QuestionEditComponent {
    title: string;
    question: Question;
    form: FormGroup;
    activityLog: string;
    //this will be true when editing an existing question and false when 
    //creating anew one
    editMode: boolean;

    constructor(private activatedRoute: ActivatedRoute,
        private http: HttpClient,
        private router: Router,
        private fb: FormBuilder,
        @Inject('BASE_URL') private baseUrl: string) 
    {
        //create an empty object from the Question interface
        this.question = <Question>{};
        this.createForm();

        var id = +this.activatedRoute.snapshot.params['id'];
        //check if we are in edit mode or not
        this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");

        if (this.editMode) {
            //fetch the quiz from the server
            var url = this.baseUrl + "api/question/" + id;
            this.http.get<Question>(url).subscribe(q => {
                this.question = q;
                this.title = "Edit - " + this.question.Text;
                this.updateForm();
            }, error => console.error(error)
            );
        }
        else {
            this.question.QuizId = id;
            this.title = "Create a new question"
        }
    }
    
    createForm(){
        this.form = this.fb.group({
            Text: ['',Validators.required]
        });

        this.activityLog = '';
        this.log('Form has beed initiated.');

        //react to the form changes
        this.form.valueChanges.subscribe(val => {
            if(!this.form.dirty){
                this.log("Form model has been loaded");
            }else{
                this.log("form was updated by the user.");
            }
        });

        this.form.get("Text")!.valueChanges.subscribe(val => {
            if(!this.form.dirty){
                this.log("Text control has been loaded with initial controls");

            }else{
                this.log("Text control was updated by the user.");
            }
        });
    }

    updateForm(){
        this.form.setValue({
            Text: this.question.Text
        });
    }

    log(str:string){
        this.activityLog+= "["+new Date().toLocaleString() + "] "+str+"<br />";
    }

    onSubmit(question:Question){
        var tempQuestion = <Question>{};
        tempQuestion.QuizId = this.question.QuizId;
        tempQuestion.Text = this.form.value.Text;
        var url = this.baseUrl + "api/question";

        if(this.editMode){
            tempQuestion.Id = this.question.Id;
            this.http.post<Question>(url,tempQuestion).subscribe(res => {
                var v = res;
                console.log("Question " + v.Id + "has been updated");
                this.router.navigate(["quiz/edit",v.QuizId]);
            },error => console.error(error))
        }
        else{
            this.http.put<Question>(url,tempQuestion).subscribe(res => {
                var v = res;
                this.router.navigate(["quiz/edit",v.QuizId])
            },error => console.error(error))
        }
    }

    onBack(){
        this.router.navigate(["quiz/edit",this.question.QuizId]);
    }
}