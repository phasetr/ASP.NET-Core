module Sample.Global.Footer

open Feliz

let private content (mainText: string) (info: (string * string) list) =
  Html.div
    [ Html.h3 [ prop.className "text-lg font-bold mb-4"; prop.text mainText ]
      Html.ul
        [ prop.className "space-y-2"
          prop.children (
            info
            |> List.map (fun (href, text) ->
              Html.li
                [ Html.a
                    [ prop.className
                        "text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100"
                      prop.href href
                      prop.text text ] ])
          ) ] ]

let private company =
  [ ("#", "About Us"); ("#", "Our Team"); ("#", "Careers"); ("#", "News") ]
  |> content "Company"

let private services =
  [ ("#", "Web Development")
    ("#", "Mobile Development")
    ("#", "Cloud Solutions")
    ("#", "Data Analytics") ]
  |> content "Services"

let private resources =
  [ ("#", "Blog"); ("#", "Community"); ("#", "Support"); ("#", "FAQs") ]
  |> content "Resources"

let private legal =
  [ ("#", "Privacy Policy"); ("#", "Terms of Service"); ("#", "Cookie Policy") ]
  |> content "Legal"

let private copyright =
  Html.div
    [ prop.className "mt-8 text-center text-gray-600 dark:text-gray-400"
      prop.text "© 2024 Acme Inc. All rights reserved." ]

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
                      prop.children [ company; services; resources; legal ] ]
                  copyright ] ] ] ]
