import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SpiderFormData } from '../models/SpiderFormData';

@Injectable({
    providedIn: 'root'
})

export class SpiderService {
    private formData = new BehaviorSubject<SpiderFormData>({
        WallWidth: 0,
        WallHeight: 0,
        SpiderX: 0,
        SpiderY: 0,
        Orientation: 'Up',
        Commands: ''
    });

    currentFormData = this.formData.asObservable();

    updateFormData(data: SpiderFormData) {
        this.formData.next(data);
    }
}