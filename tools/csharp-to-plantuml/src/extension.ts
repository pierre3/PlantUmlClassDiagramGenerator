'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import { exec } from 'child_process';
import { join as pathJoin } from 'path';
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

		const wsroot = vscode.workspace.rootPath as string;
		if (isUndefined(wsroot)) {
			console.log("Open folder or workspace.");
			return;
		}

		const outputchannel = vscode.window.createOutputChannel("CSharp to PlantUML");

		const tool = pathJoin(context.extensionPath, 'lib', 'PlantUmlClassDiagramGenerator', 'PlantUmlClassDiagramGenerator.dll');
		const conf = vscode.workspace.getConfiguration();
		const inputPath = conf.get('csharp2plantuml.inputPath') as string;
		const outputPath = conf.get('csharp2plantuml.outputPath') as string;
		const publicOnly = conf.get('csharp2plantuml.public') as boolean;
		const ignoreAccessibility = conf.get('csharp2plantuml.ignoreAccessibility') as string;
		const excludePath = conf.get('csharp2plantuml.excludePath') as string;
		const createAssociation = conf.get('csharp2plantuml.createAssociation') as boolean;
		const input = pathJoin(wsroot, inputPath);

		var command = `dotnet "${tool}" "${input}"`;
		if (outputPath !== "") {
			command += ` "${pathJoin(wsroot, outputPath)}"`;
		}
		command += " -dir";
		if (publicOnly) {
			command += " -public";
		} else if (ignoreAccessibility !== "") {
			command += ` -ignore "${ignoreAccessibility}"`;
		}
		if (excludePath !== "") {
			command += ` "${pathJoin(input, excludePath)}"`;
		}
		if (createAssociation) {
			command += " -createAssociation";
		}

		outputchannel.appendLine("[exec] " + command);
		exec(command, (error, stdout, stderror) => {
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
