import {FASTElement} from '@microsoft/fast-element';
import {ComponentTypes} from '../component-types';

export declare class AppColorBlock extends FASTElement {
  index: number;
  component: ComponentTypes;
  color: string;
  layerName?: string;
  private colorChanged;
  private updateColor;
}
