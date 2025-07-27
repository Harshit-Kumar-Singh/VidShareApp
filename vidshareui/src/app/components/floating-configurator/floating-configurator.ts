import {Component, computed, inject, input} from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { StyleClassModule } from 'primeng/styleclass';
import { AppConfigurator } from '../app.configurator';
import { LayoutService } from '../../services/layout.service';
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-floating-configurator',
  imports: [CommonModule, ButtonModule, StyleClassModule, AppConfigurator],
  templateUrl: './floating-configurator.html',
  styleUrl: './floating-configurator.scss'
})
export class FloatingConfigurator {
    LayoutService = inject(LayoutService);
    float = input<boolean>(true);
    isDarkTheme = computed(() => this.LayoutService.layoutConfig().darkTheme);
    toggleDarkMode() {
        this.LayoutService.layoutConfig.update((state) => ({ ...state, darkTheme: !state.darkTheme }));
    }
}
