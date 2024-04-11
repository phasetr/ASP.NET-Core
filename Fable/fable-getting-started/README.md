# Fable Getting Started

- [Original Repository](https://github.com/Zaid-Ajaj/fable-getting-started)

### 開発用に`watch`で実行

```shell
npm start
```

### 単純な実行

- 以下のコマンドを実行して`dist/index.html`をブラウザで開く

```shell
npm run build
```

## Original

This is the simplest Fable application you can make: it is a frontend web application with an empty page that writes `Hello from Fable` to the console.

This template is _not_ for production use and is only used to demonstrate Fable features in [The Elmish Book](https://github.com/Zaid-Ajaj/the-elmish-book)

Requirements

 - [dotnet SDK](https://dotnet.microsoft.com/en-us/download) v6.0 or later
 - [Node.js](https://nodejs.org/en/) runtime

### Installation

To compile the project, first you need to restore dotnet tools which bring it the Fable compiler
```
dotnet tool restore
```

Then run the following commands to install Node.js dependencies such as webpack and bundle the application

```bash
npm install
npm run build
```
`npm install` will install dependencies from [npm](https://www.npmjs.com/) which is the Node.js equivalent of dotnet's Nuget registry. These dependencies include the Fable compiler itself as it is distributed to npm to make compilation workflow as simple as possible.

`npm run build` is an alies for "compile with Fable, then bundle with webpack" 

After `npm run build` finished running, the generated javascript will be bundled in a single file called `main.js` located in the `dist` directory along with an existing `index.html` page that references that script file.

### Development mode

While developing the application, you don't want to recompile the application every time you make a change. Instead of that, you can start the compilation process in development mode which will watch changes you make in the file and re-compile automatically really fast:
```bash
npm install
npm start
```

If you already ran `npm install` then you don't need to run it again. `npm start` will start the developement mode by invoking `webpack-dev-server`: the webpack development server that starts a lightweight local server at http://localhost:8080 from which the server will serve the client application
