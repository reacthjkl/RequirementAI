import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import type { Chart as ChartInstance } from 'chart.js';

@Component({
  selector: 'app-quality-score-pie-chart',
  templateUrl: './quality-score-pie-chart.html',
  styleUrl: './quality-score-pie-chart.scss',
})
export class QualityScorePieChart implements AfterViewInit, OnChanges, OnDestroy {
  @Input({ required: true }) public score = 0;
  @Input() public label = 'Score';

  @ViewChild('scoreCanvas') private scoreCanvas?: ElementRef<HTMLCanvasElement>;

  private chart: ChartInstance | null = null;
  private viewReady = false;

  public ngAfterViewInit(): void {
    this.viewReady = true;
    void this.renderChart();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['score'] || changes['label']) {
      void this.renderChart();
    }
  }

  public ngOnDestroy(): void {
    this.destroyChart();
  }

  public get scoreLabel(): string {
    return `${this.normalizedScore.toFixed(1).replace(/\.0$/, '')}/10`;
  }

  private get normalizedScore(): number {
    return Math.max(0, Math.min(10, Number(this.score) || 0));
  }

  private async renderChart(): Promise<void> {
    const canvas = this.scoreCanvas?.nativeElement;

    if (!this.viewReady || !canvas) {
      return;
    }

    const primary = this.cssVariable('--bs-primary', '#27ae60');
    const danger = this.cssVariable('--bs-danger', '#dc3545');
    const border = this.cssVariable('--bs-border-color', '#dee2e6');
    const { default: Chart } = await import('chart.js/auto');
    const score = this.normalizedScore;

    this.destroyChart();
    this.chart = new Chart(canvas, {
      type: 'pie',
      data: {
        labels: [this.label, 'Remaining'],
        datasets: [
          {
            data: [score, 10 - score],
            backgroundColor: [score < 5 ? danger : primary, border],
            borderWidth: 0,
          },
        ],
      },
      options: {
        animation: false,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false,
          },
          tooltip: {
            callbacks: {
              label: (item) =>
                item.dataIndex === 0 ? `${this.label}: ${this.scoreLabel}` : '',
            },
          },
        },
      },
    });
  }

  private destroyChart(): void {
    this.chart?.destroy();
    this.chart = null;
  }

  private cssVariable(name: string, fallback: string): string {
    if (typeof document === 'undefined') {
      return fallback;
    }

    return getComputedStyle(document.documentElement).getPropertyValue(name).trim() || fallback;
  }
}
