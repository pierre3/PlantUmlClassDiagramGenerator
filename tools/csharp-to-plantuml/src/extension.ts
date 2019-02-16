'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import { exec } from 'child_process';
import { join } from 'path';
import { isUndefined } from 'util';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

	// Use the console to output diagnostic information (console.log) and errors (console.error)
	// This line of code will only be executed once when your extension is activated
	console.log('Congratulations, your extension "csharp-to-plantuml" is now active!');

	// The command has been defined in the package.json file
	// Now provide the implementation of the command with registerCommand
	// The commandId parameter must match the command field in package.json
	let disposable = vscode.commands.registerCommand('csharp2plantuml.classDiagram', () => {
		// The code you place here will be executed every time your command is executed
		const outputchannel = vscode.window.createOutputChannel("CSharp to PlantUML");

		const tool = join(context.extensionPath, 'lib', 'PlantumlClassDiagramGenerator', 'PlantUmlClassDiagramGenerator.dll');

		const input = vscode.workspace.rootPath as string;
		if(isUndefined(input)){
			console.log("Open folder or workspace.");
			return;
		}
		const output = join(input, 'diagram');
		exec(`dotnet "${tool}" "${input}" "${output}" -dir -public`, (error, stdout, stderror) => {
			outputchannel.appendLine(stdout);
			outputchannel.appendLine(stderror);
			if (error) {
				console.log(error);
			}
		});
	});

	context.subscriptions.push(disposable);
}

// this method is called when your extension is deactivated
export function deactivate() { }
