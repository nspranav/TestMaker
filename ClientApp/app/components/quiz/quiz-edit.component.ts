import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Component({
    selector: 'quiz-edit',
    templateUrl: './quiz-edit.component.html',
    styleUrls: ['./quiz-edit.component.css']
})
export class QuizEditComponent {
    title: string;
    quiz: Quiz;
    form: FormGroup;

    // this will be TRUE when editing an existing quiz,
    // false when creating a new one
    editMode: boolean;


    constructor(private activatedRoute: ActivatedRoute,
        private router: Router,
        private http: HttpClient,
        private fb: FormBuilder,
        @Inject('BASE_URL') private baseUrl: string
    ) {

        //create an empty object from Quiz interface
        this.quiz = <Quiz>{};

        //initialize the form
        this.createForm();

        var id = +this.activatedRoute.snapshot.params["id"];

        if (id) {
            this.editMode = true;

            //fetch the quiz from the server
            var url = this.baseUrl + "api/quiz/" + id;
            this.http.get<Quiz>(url).subscribe(q => {
                this.quiz = q;
                this.title = "Edit - " + this.quiz.Title;

                //update the form with quiz value
                this.updateForm();
            }, error => console.error(error));
        }
        else {
            this.editMode = false;
            this.title = "Create new Quiz";
        }
    }

    createForm(){
        this.form = this.fb.group({
            Title: ['',Validators.required],
            Description: '',
            Text: ''
        });
    }

    updateForm(){
        this.form.setValue({
            Title : this.quiz.Title,
            Description : this.quiz.Description || '',
            Text : this.quiz.Text || ''
        })
    }

    onSubmit(quiz: Quiz) {
        //build a temporary quiz object from the form
        var tempQuiz = <Quiz>{};
        tempQuiz.Title = this.form.value.Title;
        tempQuiz.Description = this.form.value.Description;
        tempQuiz.Text = this.form.value.Text;

        var url = this.baseUrl + "api/quiz";

        if (this.editMode) {
            //copy the Id from the original quiz
            tempQuiz.Id = this.quiz.Id;

            this.http.post<Quiz>(url, tempQuiz).subscribe(res => {
                var v = res;
                console.log("quiz " + v.Id + "has been updated.");
                this.router.navigate(["home"]);
            }, error => console.error(error)
            );
        } else {
            this.http.put<Quiz>(url, tempQuiz).subscribe(res => {
                var v = res;
                console.log("quiz " + v.Id + "has been created.");
                this.router.navigate(["home"]);
            }, error => console.error(error)
            );
        }
    }

    onBack() {
        this.router.navigate(["home"]);
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