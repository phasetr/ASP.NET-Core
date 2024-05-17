module Sample.WithMenu

open Feliz
open Sample.Global.Footer
open Sample.Global.Header
open Sample.Global.LeftMenu

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

let body =
  Html.div
    [ prop.className "flex-1 overflow-y-auto"
      prop.children [ header; main; footer ] ]

let myComponent =
  Html.div
    [ prop.className "flex min-h-screen"; prop.children [ leftMenu; body ] ]
