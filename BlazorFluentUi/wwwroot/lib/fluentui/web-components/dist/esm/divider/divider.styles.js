import {css} from '@microsoft/fast-element';
import {display} from '@microsoft/fast-foundation';
import {neutralStrokeDividerRest, strokeWidth} from '../design-tokens';

export const dividerStyles = (context, definition) => css`
    ${display('block')} :host {
      box-sizing: content-box;
      height: 0;
      border: none;
      border-top: calc(${strokeWidth} * 1px) solid ${neutralStrokeDividerRest};
    }
  `;