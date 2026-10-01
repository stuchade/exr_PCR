import { Routes } from '@angular/router';
import { Dashboard } from './component/dashboard/dashboard';

export const routes: Routes = [
    { path:'', component: Dashboard },
    { path:'**', redirectTo: '' }
];
