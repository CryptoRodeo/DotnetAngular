import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrls: ['./health-check.component.css']
})
export class HealthCheckComponent implements OnInit {
  public result: Result;
  constructor(
    private http: HttpClient,
    //Get this provider
    @Inject('BASE_URL') private baseUrl: string
  ) { }

  ngOnInit(): void {
    this.http.get<Result>(this.baseUrl + 'hc').subscribe(res => {
      this.result = res;
    }, err => console.error(err));
  }
}

interface Result {
  checks: Check[];
  totalStatus: string;
  totalResponseTime: number;
}

interface Check {
  name: string;
  status: string;
  responseTime: number;
}
