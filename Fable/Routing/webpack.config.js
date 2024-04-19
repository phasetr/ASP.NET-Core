var path = require("path");

module.exports = {
  mode: "development",
  entry: "./src/Main.fs.js",
  output: {
    path: path.resolve(__dirname, "./dist"),
    filename: "main.js"
  },
  devServer: {
    static: {
      directory: path.resolve(__dirname, "dist")
    }
  }
}
