module Sample.Global.Footer

open Feliz

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
