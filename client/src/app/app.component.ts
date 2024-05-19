import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { IProduct } from './models/product';
import { IPagination } from './models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'Layal';
  products: IProduct[] = []; // Initialize products as an empty array
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http
      .get<IPagination>('https://localhost:5123/api/products?pageSize=50')
      .subscribe(
        (response) => {
          this.products = response.data;
        },
        (error) => {
          console.log(error);
        }
      );
  }
}
