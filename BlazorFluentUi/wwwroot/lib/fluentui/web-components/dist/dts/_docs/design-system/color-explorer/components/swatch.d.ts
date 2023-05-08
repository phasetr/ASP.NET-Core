import {DesignToken, FoundationElement} from '@microsoft/fast-foundation';
import {Swatch} from '../../../../index-rollup';

export declare enum SwatchTypes {
  fill = "fill",
  foreground = "foreground",
  outline = "outline"
}

export declare class AppSwatch extends FoundationElement {
  type: SwatchTypes;
  recipeName: string;
  foregroundRecipe?: DesignToken<Swatch>;
  fillRecipe?: DesignToken<Swatch>;
  outlineRecipe?: DesignToken<Swatch>;
  iconStyle: string;
  contrastMessage: string;
  colorValue: string;
  private updateObservables;
  private tokenCSS;
  private evaluateToken;
  private updateIconStyle;
  private formatContrast;
  private formatBackgroundContrast;
  private formatForegroundContrast;
  private updateContrastMessage;
  private updateColorValue;

  foregroundRecipeChanged(): void;

  fillRecipeChanged(): void;

  outlineRecipeChanged(): void;

  connectedCallback(): void;
}
