module Sample.Global.Header

open Feliz
open Feliz.Router
open Sample.Global.Svg

let acmeInc =
  Html.div
    [ prop.className "flex items-center"
      prop.children
        [ Html.button
            [ prop.className "lg:hidden mr-4"
              prop.children [ svgHamburgerMenu ] ]
          Html.a
            [ prop.className "flex items-center"
              prop.href (Router.format "/")
              prop.children
                [ svgAcmeInc
                  Html.span
                    [ prop.className
                        "text-lg font-semibold text-gray-900 dark:text-gray-100"
                      prop.text "Acme Inc" ] ] ] ] ]

let jpSpan =
  Html.span
    [ prop.className
        "relative flex shrink-0 overflow-hidden rounded-full h-9 w-9"
      prop.custom ("type", "button")
      prop.custom ("id", "radix-:r8:")
      prop.ariaHasPopup true
      prop.ariaExpanded false
      prop.custom ("data-state", "closed")
      prop.children
        [ Html.span
            [ prop.className
                "flex h-full w-full items-center justify-center rounded-full bg-muted"
              prop.text "JP" ] ] ]

let search =
  Html.div
    [ prop.className "flex items-center"
      prop.children
        [ Html.a
            [ prop.className
                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100 mr-4"
              prop.href "#"
              prop.children [ svgSearch ] ]
          jpSpan ] ]

let header =
  Html.header
    [ prop.className "bg-white dark:bg-gray-900 shadow"
      prop.children
        [ Html.div
            [ prop.className
                "max-w-7xl mx-auto py-4 px-4 sm:px-6 lg:px-8 flex items-center justify-between"
              prop.children [ acmeInc; search ] ] ] ]
