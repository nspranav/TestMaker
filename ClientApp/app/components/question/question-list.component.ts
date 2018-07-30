import { Component, OnChanges, Input, Inject, SimpleChanges } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";

@Component({
    selector: 'question-list',
    templateUrl: './question-list.component.html',
    styleUrls: ["./question-list.component.css"]
})
export class QuestionListComponent implements OnChanges{
    @Input() quiz: Quiz;
    questions: Question[];
    title: string;

    constructor(private http: HttpClient, 
        @Inject('BASE_URL') private baseUrl: string,
        private router: Router){
            this.questions =[];
    }

    ngOnChanges(changes: SimpleChanges){
        if(typeof changes['quiz'] !== 'undefined'){
            // retrieve the quiz variable change info
            var change = changes['quiz'];

            //only perform the task if the value has been changed
            if(!change.isFirstChange()){
                //execute the http request and retieve the result
                this.loadData();
            }
        }
    }

    loadData(){
        var url = this.baseUrl + "api/question/All/" + this.quiz.Id;
        this.http.get<Question[]>(url).subscribe( q => {this.questions = q },
             error => console.error(error));
    }

    onCreate(){
        this.router.navigate(["/question/create",this.quiz.Id]);
    }

    onEdit(question: Question){
        this.router.navigate(["/question/edit",question.Id]);
    }

    onDelete(question: Question){
        if(confirm("Do you want to delete this question?")){
            var url = this.baseUrl + "/api/question/" + question.Id;
            this.http.delete(url).subscribe(res => {
                console.log(`question with id ${question.Id} is deleted`);
                this.loadData();
            },error => console.error(error)
            );

        }
    }
}