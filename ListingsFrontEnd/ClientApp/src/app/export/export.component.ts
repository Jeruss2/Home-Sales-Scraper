import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-export',
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.css']
})
export class ExportComponent implements OnInit {
  constructor(private http: HttpClient) {}

  public document: any;

  //public ExportListings() {
  //  //api/Listings/Export

  //  this.http.get("api/Export").subscribe(result => { this.document = result; console.log(result) });

  //}

  ngOnInit() {
  }

  public ExportListings() {
    this.http.get('api/Export', { responseType: 'blob' }).subscribe(blob => {
      saveAs(blob,
        'export_data.txt',
        {
          type: 'text/plain;charset=windows-1252' // --> or whatever you need here
        });
    });
  }
}
