import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { QuoteService } from '../../shared/services/quote.service';
import { QuoteResponse } from '../../shared/models/models';

@Component({
  selector: 'app-quote-list',
  imports: [
    CommonModule,
    RouterModule,
    NzTableModule,
    NzButtonModule,
    NzCardModule,
  ],
  templateUrl: './quote-list.component.html',
  styleUrl: './quote-list.component.css',
})
export class QuoteListComponent implements OnInit {
  quotes: QuoteResponse[] = [];
  loading = false;

  constructor(
    private quoteService: QuoteService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadQuotes();
  }

  loadQuotes(): void {
    this.loading = true;
    this.quoteService.getAllQuotes().subscribe({
      next: (quotes) => {
        this.quotes = quotes;
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        this.message.error('Failed to load quotes');
      },
    });
  }
}
