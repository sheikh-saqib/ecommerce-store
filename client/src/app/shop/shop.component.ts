import { Component } from '@angular/core';
import { IProduct } from '../models/product';
import { ShopService } from './shop.service';
import { error } from 'console';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent {
  products: IProduct[] | undefined;

  constructor(private shopService: ShopService) {}

  ngOnInit() {
    this.shopService.getProducts().subscribe(
      (response) => {
        this.products = response.data;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
