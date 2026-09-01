import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/mission-list/mission-list').then((m) => m.MissionList),
  },
  {
    path: 'missions/:id',
    loadComponent: () =>
      import('./features/mission-assign/mission-assign').then((m) => m.MissionAssign),
  },
  { path: '**', redirectTo: '' },
];
