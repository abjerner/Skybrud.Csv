module.exports = function (grunt) {

    var path = require('path');

	// Load the package JSON file
	var pkg = grunt.file.readJSON('package.json');

	// Load information about the assembly
    var assembly = grunt.file.readJSON('src/Skybrud.Csv/Properties/AssemblyInfo.json');

	// Get the version of the package
    var version = assembly.informationalVersion ? assembly.informationalVersion : assembly.version;

	grunt.initConfig({
		pkg: pkg,
		nugetpack: {
			hest: {
				src: 'src/Skybrud.Csv/Skybrud.Csv.csproj',
				dest: 'releases/nuget/'
			}
		},
		zip: {
			release: {
				router: function (filepath) {
					return path.basename(filepath);
				},
				src: [
					'src/Skybrud.Social.Core/bin/Release/Skybrud.Csv.dll',
					'src/Skybrud.Social.Core/bin/Release/Skybrud.Csv.xml'
				],
				dest: 'releases/github/Skybrud.Csv.v' + version + '.zip'
			}
		}
	});

	grunt.loadNpmTasks('grunt-nuget');
	grunt.loadNpmTasks('grunt-zip');

	grunt.registerTask('release', ['nugetpack', 'zip']);

	grunt.registerTask('default', ['release']);

};