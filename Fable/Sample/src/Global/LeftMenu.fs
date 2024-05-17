module Sample.Global.LeftMenu

open Feliz
open Sample.Global.Svg

let leftMenu =
  Html.div
    [ prop.className
        "hidden lg:block bg-gray-100 dark:bg-gray-800 w-64 p-6 border-r border-gray-200 dark:border-gray-700"
      prop.children
        [ Html.div
            [ prop.className "flex items-center mb-6"
              prop.children
                [ svgAcmeInc
                  Html.span
                    [ prop.className "text-lg font-semibold"
                      prop.text "Acme Inc" ] ] ]
          Html.nav
            [ prop.className "space-y-1"
              prop.children
                [ Html.a
                    [ prop.className
                        "flex items-center px-3 py-2 text-gray-900 rounded-md bg-gray-200 dark:bg-gray-700 dark:text-gray-100 font-medium"
                      prop.href ""
                      prop.children [ svgHome; Html.text "Home" ] ]
                  Html.a
                    [ prop.className
                        "flex items-center px-3 py-2 text-gray-600 hover:bg-gray-200 hover:text-gray-900 rounded-md dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-gray-100 font-medium"
                      prop.href "about"
                      prop.children [ svgAbout; Html.text "About" ] ]
                  Html.a
                    [ prop.className
                        "flex items-center px-3 py-2 text-gray-600 hover:bg-gray-200 hover:text-gray-900 rounded-md dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-gray-100 font-medium"
                      prop.href "services"
                      prop.children [ svgService; Html.text "Services" ] ]
                  Html.a
                    [ prop.className
                        "flex items-center px-3 py-2 text-gray-600 hover:bg-gray-200 hover:text-gray-900 rounded-md dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-gray-100 font-medium"
                      prop.href "contact"
                      prop.children [ svgContact; Html.text "Contact" ] ] ] ] ] ]
