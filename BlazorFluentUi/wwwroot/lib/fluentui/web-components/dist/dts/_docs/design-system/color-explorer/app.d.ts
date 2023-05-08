import {FASTElement} from '@microsoft/fast-element';
import {ComponentTypes} from './component-types';

export interface SwatchInfo {
  index: number;
  color: string;
  title?: string;
}

export declare class App extends FASTElement {
  canvasElement: HTMLDivElement;
  componentType: ComponentTypes;
  neutralColor: string;
  neutralPalette: string[];
  accentColor: string;
  accentPalette: string[];
  showOnlyLayerBackgrounds: boolean;
  private neutralColorChanged;
  private accentColorChanged;
  private layerRecipes;
  private resolveLayerRecipes;

  private get lightModeLayers();

  private get darkModeLayers();

  connectedCallback(): void;

  backgrounds(): Array<SwatchInfo>;

  controlPaneHandler(e: CustomEvent): void;
}
