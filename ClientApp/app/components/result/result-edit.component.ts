import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Component({
    selector: "result-edit",
    templateUrl: "./result-edit.component.html",
    styleUrls: ["./result-edit.component.css"]
})
export class ResultEditComponent {
    title: string;
    result: Result;
    form: FormGroup;

    //this will be true when editing an existing result and false when 
    //creating anew one
    editMode: boolean;

    constructor(private activatedRoute: ActivatedRoute,
        private http: HttpClient,
        private router: Router,
        private fb: FormBuilder,
        @Inject('BASE_URL') private baseUrl: string) 
    {
        this.createForm();
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
                //update the form model
                this.updateForm();
            }, error => console.error(error)
            );
        }
        else {
            this.result.QuizId = id;
            this.title = "Create a new result"
        }
    }

    createForm(){
        this.form = this.fb.group({
            'Text': ['',Validators.required],
            'MinValue': ['',Validators.pattern(/^\d*$/)],
            'MaxValue': ['',Validators.pattern(/^\d*$/)]
        })
    }

    updateForm(){
        this.form.setValue({
            'Text': this.result.Text,
            'MaxValue': this.result.MaxValue || '',
            'MinValue': this.result.MinValue || '' 
        })
    }

    onSubmit(result:Result){
        var tempResult = <Result>{};
        tempResult.QuizId = this.result.QuizId;
        tempResult.MinValue = this.form.value.MinValue;
        tempResult.MaxValue = this.form.value.MaxValue;
        var url = this.baseUrl + "api/result";

        if(this.editMode){
            tempResult.Id = this.result.Id;
            this.http.post<Result>(url,tempResult).subscribe(res => {
                var v = res;
                console.log("Result " + v.Id + "has been updated");
                this.router.navigate(["quiz/edit",v.QuizId]);
            },error => console.error(error))
        }
        else{
            this.http.put<Result>(url,tempResult).subscribe(res => {
                var v = res;
                this.router.navigate(["quiz/edit",v.QuizId])
            },error => console.error(error))
        }
    }

    onBack(){
        this.router.navigate(["quiz/edit",this.result.QuizId]);
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