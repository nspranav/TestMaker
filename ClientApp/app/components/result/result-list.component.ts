import { Component, OnChanges, Input, Inject, SimpleChanges } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";

@Component({
    selector: 'result-list',
    templateUrl: './result-list.component.html',
    styleUrls: ["./result-list.component.css"]
})
export class ResultListComponent implements OnChanges{
    @Input() quiz: Quiz;
    results: Result[];
    title: string;

    constructor(private http: HttpClient, 
        @Inject('BASE_URL') private baseUrl: string,
        private router: Router){
            this.results =[];
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
        var url = this.baseUrl + "api/result/All/" + this.quiz.Id;
        this.http.get<Result[]>(url).subscribe( q => {this.results = q },
             error => console.error(error));
    }

    onCreate(){
        this.router.navigate(["/result/create",this.quiz.Id]);
    }

    onEdit(result: Result){
        this.router.navigate(["/result/edit",result.Id]);
    }

    onDelete(result: Result){
        if(confirm("Do you want to delete this result?")){
            var url = this.baseUrl + "/api/result/" + result.Id;
            this.http.delete(url).subscribe(res => {
                console.log(`result with id ${result.Id} is deleted`);
                this.loadData();
            },error => console.error(error)
            );

        }
    }
}