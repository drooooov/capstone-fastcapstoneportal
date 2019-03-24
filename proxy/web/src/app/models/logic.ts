import { Entreaty } from './entreaty.model';

export class Logic {

  static mapInfo(data: Entreaty[]): Entreaty[] {
    data = data.map(x => {
      if (x.entreatyType === 0) {
        x.title = 'Request';
      } else if (x.entreatyType === 1) {
        x.title = 'Group Invite';
      }
      return x;
    });
    return data;
  }
}
