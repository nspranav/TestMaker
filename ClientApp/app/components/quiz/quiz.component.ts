import { Component, Inject } from "@angular/core";
import { ActivatedRoute,Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
    selector: "quiz",
    templateUrl: "./quiz.component.html",
    styleUrls: ['./quiz.component.css']
})
export class QuizComponent{
    quiz: Quiz;

    /**
     *
     */
    constructor(private activatedRoute: ActivatedRoute, 
        private router: Router, 
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string) 
        {
        
            //create an empty object from quiz interface
            this.quiz = <Quiz>{};

            var id = +this.activatedRoute.snapshot.params['id'];
            console.log(id);

            if(id){
                var url = baseUrl + "api/quiz/" + id;
                
                this.http.get<Quiz>(url).subscribe(result  => {
                    this.quiz = result;
                    
                }, error => console.error(error));

            }
            else{
                console.log("invalid id: routing back to home");
                this.router.navigate(["home"]);
            }
            
        }

        onEdit(){
            this.router.navigate(["quiz/edit", this.quiz.Id])
        }

        onDelete(){
            if(confirm("Do you wnat to delete this quiz?")){
                var url = this.baseUrl + "api/quiz/" + this.quiz.Id;
                this.http.delete(url).subscribe(res => {
                    console.log("Quiz "+ this.quiz.Id+ " has been deleted.");
                    this.router.navigate(["home"]);
                },e => console.error(e));
            }
        }
}