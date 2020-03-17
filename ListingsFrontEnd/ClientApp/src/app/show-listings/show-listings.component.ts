import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-show-listings',
  templateUrl: './show-listings.component.html',
  styleUrls: ['./show-listings.component.css']
})
export class ShowListingsComponent implements OnInit {
  constructor(private http: HttpClient) { }

  public listings:Listing[];


  //public listing:Listing = new Listing();


  ngOnInit() {
    this.ShowListing();
  }

  public ShowListing(): boolean {
    console.log("hey im here!");

    this.http.get<Listing[]>("api/Listings").subscribe(result => {this.listings = result; console.log(result)});

    //this.http.post("https://localhost:4200/api/Listings", this.listing).subscribe(result => {console.log(result)});

    return true;
  }
}

export class Listing {
  neighborhood: string;
  address: string;
  price: string;
  numBeds: string;
  numBath: string;
}
