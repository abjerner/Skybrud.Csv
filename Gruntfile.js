module.exports = function (grunt) {

    var path = require('path');

	// Load the package JSON file
	var pkg = grunt.file.readJSON('package.json');

	// Get the root path of the project
	var projectRoot = 'src/' + pkg.name + '/';

	// Load information about the assembly
    var assembly = grunt.file.readJSON('src/Skybrud.Csv/Properties/AssemblyInfo.json');

	// Get the version of the package
    var version = assembly.informationalVersion ? assembly.informationalVersion : assembly.version;

	grunt.initConfig({
		pkg: pkg,
		clean: {
			files: [
				'releases/temp/'
			]
		},
		copy: {
			bacon: {
				files: [
					{
						expand: true,
						cwd: projectRoot + '../',
						src: [
							'LICENSE.html'
						],
						dest: 'releases/temp/'
					},
					{
						expand: true,
						cwd: projectRoot + 'bin/Release/',
						src: [
							'**/*.dll',
							'**/*.xml'
						],
						dest: 'releases/temp/'
					}
				]
			}
		},
		zip: {
			release: {
				cwd: 'releases/temp/',
				src: [
					'releases/temp/**/*.*'
				],
				dest: 'releases/github/' + pkg.name + '.v' + version + '.zip'
			}
		}
	});

	grunt.loadNpmTasks('grunt-contrib-clean');
	grunt.loadNpmTasks('grunt-contrib-copy');
	grunt.loadNpmTasks('grunt-zip');

	grunt.registerTask('release', ['clean', 'copy', 'zip', 'clean']);

	grunt.registerTask('default', ['release']);

};