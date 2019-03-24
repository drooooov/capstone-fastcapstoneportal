import { Group } from './../models/group.model';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class  SpecificGroupService {

  private _url: String = 'assets/data/';
  constructor(private http: HttpClient) { }


  getGroup(id: String): any {

    let groupList: Group[];

    this.http.get<Group[]>(this._url + 'groups.json').subscribe(
      (data) => { groupList = data; },
      (err) => { },
      () => {
        groupList.forEach(element => {
          if (element.id === id) {
            return element;
          } else {
            return {};
          }
        });
      });
  }
}
