import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Listing } from "../show-listings/show-listings.component";

@Component({
  selector: 'app-search-listings',
  templateUrl: './search-listings.component.html',
  styleUrls: ['./search-listings.component.css']
})
export class SearchListingsComponent implements OnInit {
constructor(private http: HttpClient) { }


public neighborhoodSearchParam: string = "";


public listings: Listing[];

  ngOnInit() {
    this.GetListingByNeighborhood();
  }
  

  public GetListingByNeighborhood(): boolean {

    this.http.get<Listing[]>("api/Listings/" + this.neighborhoodSearchParam).subscribe(result => { this.listings = result; console.log(result) });


    return true;
  }
  

}
