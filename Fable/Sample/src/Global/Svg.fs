module Sample.Global.Svg

open Feliz

let svgFundamentals =
  [ svg.xmlns "http://www.w3.org/2000/svg"
    svg.width 24
    svg.height 24
    svg.custom ("data-darkreader-inline-stroke", "currentColor")
    svg.viewBox (0, 0, 24, 24)
    svg.fill "none"
    svg.stroke "currentColor"
    svg.strokeWidth 2
    svg.strokeLineCap "round"
    svg.strokeLineJoin "round" ]

let svgAcmeInc =
  svgFundamentals
  |> List.append
    [ svg.className "h-6 w-6 mr-2 text-gray-900 dark:text-gray-100"
      svg.children [ Svg.path [ svg.d "m8 3 4 8 5-5 5 15H2L8 3z" ] ] ]
  |> Svg.svg

let svgHome =
  svgFundamentals
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.children
        [ Svg.path [ svg.d "m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z" ]
          Svg.polyline [ svg.points "9 22 9 12 15 12 15 22" ] ] ]
  |> Svg.svg

let svgAbout =
  svgFundamentals
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.children
        [ Svg.path [ svg.d "M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2" ]
          Svg.circle [ svg.cx 12; svg.cy 7; svg.r 4 ] ] ]
  |> Svg.svg

let svgService =
  svgFundamentals
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.children
        [ Svg.path [ svg.d "M16 20V4a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16" ]
          Svg.rect [ svg.width 20; svg.height 14; svg.x 2; svg.y 6; svg.rx 2 ] ] ]
  |> Svg.svg

let svgContact =
  svgFundamentals
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.custom ("data-darkreader-inline-stroke", "currentColor")
      svg.children
        [ Svg.rect [ svg.width 20; svg.height 16; svg.x 2; svg.y 4; svg.rx 2 ]
          Svg.path [ svg.d "m22 7-8.97 5.7a1.94 1.94 0 0 1-2.06 0L2 7" ] ] ]
  |> Svg.svg

let svgHamburgerMenu =
  svgFundamentals
  |> List.append
    [ svg.className "h-6 w-6 text-gray-600 dark:text-gray-400"
      svg.children
        [ Svg.line [ svg.x1 4; svg.x2 20; svg.y1 12; svg.y2 12 ]
          Svg.line [ svg.x1 4; svg.x2 20; svg.y1 6; svg.y2 6 ]
          Svg.line [ svg.x1 4; svg.x2 20; svg.y1 18; svg.y2 18 ] ] ]
  |> Svg.svg

let svgSearch =
  svgFundamentals
  |> List.append
    [ svg.className "h-6 w-6"
      svg.children
        [ Svg.circle [ svg.cx 11; svg.cy 11; svg.r 8 ]
          Svg.path [ svg.d "m21 21-4.3-4.3" ] ] ]
  |> Svg.svg
