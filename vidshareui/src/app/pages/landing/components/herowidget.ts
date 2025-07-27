import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
// import {image} from '../../../../assets/images/'
@Component({
    selector: 'hero-widget',
    imports: [ButtonModule, RippleModule],
    template: `
        <div
            id="hero"
            class="flex flex-col pt-6 px-6 lg:px-20 overflow-hidden"
            style="background: linear-gradient(0deg, rgba(255, 255, 255, 0.2), rgba(255, 255, 255, 0.2)), radial-gradient(77.36% 256.97% at 77.36% 57.52%, rgb(238, 239, 175) 0%, rgb(195, 227, 250) 100%); clip-path: ellipse(150% 87% at 93% 13%)"
        >
            <div class="mx-6 md:mx-20 mt-0 md:mt-6">
                <h1 class="text-6xl font-bold text-gray-900 leading-tight dark:!text-gray-700"><span class="font-light block">V-Share</span>Share Your World</h1>
                <p class="font-normal text-2xl leading-normal md:mt-4 text-gray-700 dark:text-gray-700">V-Share is the easiest way to upload and share videos</p>
                <button pButton pRipple [rounded]="true" type="button" label="Get Started" class="text-xl! mt-8 px-4!"></button>
            </div>
            <div class="flex justify-center md:justify-end">
                <img src="assets/images/Designer.png" alt="Hero Image" class="w-130 aspect-square object-cover" />
            </div>
        </div>
    `
})
export class HeroWidget {}
