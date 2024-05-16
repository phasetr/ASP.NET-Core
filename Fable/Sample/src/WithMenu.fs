module Sample.WithMenu

open Feliz

let svgProperties =
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
  svgProperties
  |> List.append
    [ svg.className "h-6 w-6 mr-2"
      svg.children [ Svg.path [ svg.d "m8 3 4 8 5-5 5 15H2L8 3z" ] ] ]
  |> Svg.svg

let svgHome =
  svgProperties
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.children
        [ Svg.path [ svg.d "m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z" ]
          Svg.polyline [ svg.points "9 22 9 12 15 12 15 22" ] ] ]
  |> Svg.svg

let svgAbout =
  svgProperties
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.children
        [ Svg.path [ svg.d "M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2" ]
          Svg.circle [ svg.cx 12; svg.cy 7; svg.r 4 ] ] ]
  |> Svg.svg

let svgService =
  svgProperties
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.children
        [ Svg.path [ svg.d "M16 20V4a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16" ]
          Svg.rect [ svg.width 20; svg.height 14; svg.x 2; svg.y 6; svg.rx 2 ] ] ]
  |> Svg.svg

let svgContact =
  svgProperties
  |> List.append
    [ svg.className "h-5 w-5 mr-3"
      svg.custom ("data-darkreader-inline-stroke", "currentColor")
      svg.children
        [ Svg.rect [ svg.width 20; svg.height 16; svg.x 2; svg.y 4; svg.rx 2 ]
          Svg.path [ svg.d "m22 7-8.97 5.7a1.94 1.94 0 0 1-2.06 0L2 7" ] ] ]
  |> Svg.svg

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

let header =
  Html.header
    [ prop.className "bg-white dark:bg-gray-900 shadow"
      prop.children
        [ Html.div
            [ prop.className
                "max-w-7xl mx-auto py-4 px-4 sm:px-6 lg:px-8 flex items-center justify-between"
              prop.children
                [ Html.div
                    [ prop.className "flex items-center"
                      prop.children
                        [ Html.button
                            [ prop.className "lg:hidden mr-4"
                              prop.children
                                [ Svg.svg
                                    [ svg.xmlns "http://www.w3.org/2000/svg"
                                      svg.width 24
                                      svg.height 24
                                      svg.viewBox (0, 0, 24, 24)
                                      svg.fill "none"
                                      svg.stroke "currentColor"
                                      svg.strokeWidth 2
                                      svg.strokeLineCap "round"
                                      svg.strokeLineJoin "round"
                                      svg.className
                                        "h-6 w-6 text-gray-600 dark:text-gray-400"
                                      svg.custom (
                                        "data-darkreader-inline-stroke",
                                        "currentColor"
                                      )
                                      svg.children
                                        [ Svg.line
                                            [ svg.x1 4
                                              svg.x2 20
                                              svg.y1 12
                                              svg.y2 12 ]
                                          Svg.line
                                            [ svg.x1 4
                                              svg.x2 20
                                              svg.y1 6
                                              svg.y2 6 ]
                                          Svg.line
                                            [ svg.x1 4
                                              svg.x2 20
                                              svg.y1 18
                                              svg.y2 18 ] ] ] ] ]
                          Html.a
                            [ prop.className "flex items-center"
                              prop.href "#"
                              prop.children
                                [ Svg.svg
                                    [ svg.xmlns "http://www.w3.org/2000/svg"
                                      svg.width 24
                                      svg.height 24
                                      svg.viewBox (0, 0, 24, 24)
                                      svg.fill "none"
                                      svg.stroke "currentColor"
                                      svg.strokeWidth 2
                                      svg.strokeLineCap "round"
                                      svg.strokeLineJoin "round"
                                      svg.className "h-6 w-6 mr-2"
                                      svg.custom (
                                        "data-darkreader-inline-stroke",
                                        "currentColor"
                                      )
                                      svg.children
                                        [ Svg.path
                                            [ svg.d "m8 3 4 8 5-5 5 15H2L8 3z" ] ] ]
                                  Html.span
                                    [ prop.className "text-lg font-semibold"
                                      prop.text "Acme Inc" ] ] ] ] ]
                  Html.div
                    [ prop.className "flex items-center"
                      prop.children
                        [ Html.a
                            [ prop.className
                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100 mr-4"
                              prop.href "#"
                              prop.children
                                [ Svg.svg
                                    [ svg.xmlns "http://www.w3.org/2000/svg"
                                      svg.width 24
                                      svg.height 24
                                      svg.viewBox (0, 0, 24, 24)
                                      svg.fill "none"
                                      svg.stroke "currentColor"
                                      svg.strokeWidth 2
                                      svg.strokeLineCap "round"
                                      svg.strokeLineJoin "round"
                                      svg.className "h-6 w-6"
                                      svg.custom (
                                        "data-darkreader-inline-stroke",
                                        "currentColor"
                                      )
                                      svg.children
                                        [ Svg.circle
                                            [ svg.cx 11; svg.cy 11; svg.r 8 ]
                                          Svg.path [ svg.d "m21 21-4.3-4.3" ] ] ] ] ]
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
                                      prop.text "JP" ] ] ] ] ] ] ] ] ]

let main =
  Html.main
    [ prop.className "py-8 px-4 sm:px-6 lg:px-8"
      prop.children
        [ Html.div
            [ prop.className "max-w-7xl mx-auto"
              prop.children
                [ Html.h1
                    [ prop.className "text-3xl font-bold mb-4"
                      prop.text "Welcome to Acme Inc" ]
                  Html.p
                    [ prop.className "text-gray-600 dark:text-gray-400 mb-8"
                      prop.text
                        "We are a leading provider of innovative solutions for businesses of all sizes." ]
                  Html.div
                    [ prop.className
                        "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8"
                      prop.children
                        [ Html.div
                            [ prop.className
                                "bg-white dark:bg-gray-900 rounded-lg shadow p-6"
                              prop.children
                                [ Html.h2
                                    [ prop.className "text-xl font-bold mb-4"
                                      prop.text "Services" ]
                                  Html.ul
                                    [ prop.className "space-y-2"
                                      prop.children
                                        [ Html.li
                                            [ Html.a
                                                [ prop.className
                                                    "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                                  prop.href "#"
                                                  prop.text "Web Development" ] ]
                                          Html.li
                                            [ Html.a
                                                [ prop.className
                                                    "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                                  prop.href "#"
                                                  prop.text "Mobile Development" ] ]
                                          Html.li
                                            [ Html.a
                                                [ prop.className
                                                    "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                                  prop.href "#"
                                                  prop.text "Cloud Solutions" ] ]
                                          Html.li
                                            [ Html.a
                                                [ prop.className
                                                    "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                                  prop.href "#"
                                                  prop.text "Data Analytics" ] ] ] ] ] ]
                          Html.div
                            [ prop.className
                                "bg-white dark:bg-gray-900 rounded-lg shadow p-6"
                              prop.children
                                [ Html.h2
                                    [ prop.className "text-xl font-bold mb-4"
                                      prop.text "About Us" ]
                                  Html.p
                                    [ prop.className
                                        "text-gray-600 dark:text-gray-400 mb-4"
                                      prop.text
                                        "Acme Inc is a leading provider of innovative solutions for businesses of all sizes. We have been in the industry for over 20 years and have a team of experienced professionals who are dedicated to delivering the best possible service to our clients." ]
                                  Html.a
                                    [ prop.className
                                        "text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                                      prop.href "#"
                                      prop.text "Learn More" ] ] ]
                          Html.div
                            [ prop.className
                                "bg-white dark:bg-gray-900 rounded-lg shadow p-6"
                              prop.children
                                [ Html.h2
                                    [ prop.className "text-xl font-bold mb-4"
                                      prop.text "Contact Us" ]
                                  Html.p
                                    [ prop.className
                                        "text-gray-600 dark:text-gray-400 mb-4"
                                      prop.text
                                        "If you have any questions or would like to learn more about our services, please don't hesitate to contact us." ]
                                  Html.a
                                    [ prop.className
                                        "text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                                      prop.href "#"
                                      prop.text "Get in Touch" ] ] ] ] ] ] ] ] ]

let footer =
  Html.footer
    [ prop.className "bg-gray-100 dark:bg-gray-800 py-8 px-4 sm:px-6 lg:px-8"
      prop.children
        [ Html.div
            [ prop.className "max-w-7xl mx-auto"
              prop.children
                [ Html.div
                    [ prop.className
                        "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8"
                      prop.children
                        [ Html.div
                            [ Html.h3
                                [ prop.className "text-lg font-bold mb-4"
                                  prop.text "Company" ]
                              Html.ul
                                [ prop.className "space-y-2"
                                  prop.children
                                    [ Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "About Us" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Our Team" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Careers" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "News" ] ] ] ] ]
                          Html.div
                            [ Html.h3
                                [ prop.className "text-lg font-bold mb-4"
                                  prop.text "Services" ]
                              Html.ul
                                [ prop.className "space-y-2"
                                  prop.children
                                    [ Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Web Development" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Mobile Development" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Cloud Solutions" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Data Analytics" ] ] ] ] ]
                          Html.div
                            [ Html.h3
                                [ prop.className "text-lg font-bold mb-4"
                                  prop.text "Resources" ]
                              Html.ul
                                [ prop.className "space-y-2"
                                  prop.children
                                    [ Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Blog" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Community" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Support" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "FAQs" ] ] ] ] ]
                          Html.div
                            [ Html.h3
                                [ prop.className "text-lg font-bold mb-4"
                                  prop.text "Legal" ]
                              Html.ul
                                [ prop.className "space-y-2"
                                  prop.children
                                    [ Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Privacy Policy" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Terms of Service" ] ]
                                      Html.li
                                        [ Html.a
                                            [ prop.className
                                                "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                                              prop.href "#"
                                              prop.text "Cookie Policy" ] ] ] ] ] ] ]
                  Html.div
                    [ prop.className
                        "mt-8 text-center text-gray-600 dark:text-gray-400"
                      prop.text "© 2024 Acme Inc. All rights reserved." ] ] ] ] ]

let body =
  Html.div
    [ prop.className "flex-1 overflow-y-auto"
      prop.children [ header; main; footer ] ]

let myComponent =
  Html.div
    [ prop.className "flex min-h-screen"
      prop.children [ leftMenu; body ] ]
