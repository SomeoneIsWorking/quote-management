import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzAutocompleteModule } from 'ng-zorro-antd/auto-complete';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { QuoteService } from '../../shared/services/quote.service';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-quote-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzCardModule,
    NzSelectModule,
    NzAutocompleteModule,
    NzInputNumberModule,
  ],
  templateUrl: './quote-form.component.html',
  styleUrl: './quote-form.component.css',
})
export class QuoteFormComponent implements OnInit {
  quoteForm: FormGroup;
  loading = false;
  cities: string[] = [];
  loadingCities = false;
  usdAmount: number | null = null;

  modeOptions = ['LCL', 'FCL', 'Air'];
  movementTypeOptions = [
    'Door to Door',
    'Port to Door',
    'Door to Port',
    'Port to Port',
  ];
  incotermsOptions = ['DDP', 'DAT'];
  packageTypeOptions = ['Pallets', 'Boxes', 'Cartons'];
  currencyOptions = ['USD', 'CNY', 'TRY'];

  constructor(
    private fb: FormBuilder,
    private quoteService: QuoteService,
    private router: Router,
    private message: NzMessageService
  ) {
    this.quoteForm = this.fb.group({
      mode: ['', Validators.required],
      movementType: ['', Validators.required],
      incoterms: ['', Validators.required],
      packageType: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required],
      currency: ['USD', Validators.required],
      amount: [null, [Validators.required, Validators.min(0.01)]],
    });
  }

  ngOnInit(): void {
    // Watch country changes for city autocomplete
    this.quoteForm
      .get('country')
      ?.valueChanges.pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((country) => {
          if (country && country.length >= 2) {
            this.loadingCities = true;
            return this.quoteService.getCities(country);
          }
          return of([]);
        })
      )
      .subscribe({
        next: (cities) => {
          this.cities = cities;
          this.loadingCities = false;
        },
        error: () => {
          this.loadingCities = false;
          this.cities = [];
        },
      });
  }

  onSubmit(): void {
    if (this.quoteForm.valid) {
      this.loading = true;
      this.quoteService.createQuote(this.quoteForm.value).subscribe({
        next: (response) => {
          this.message.success('Quote request created successfully!');
          this.router.navigate(['/quotes/list']);
        },
        error: (error) => {
          this.loading = false;
          this.message.error(
            error.error?.message || 'Failed to create quote request.'
          );
        },
      });
    } else {
      Object.values(this.quoteForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
