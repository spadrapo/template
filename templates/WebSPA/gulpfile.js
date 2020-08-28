﻿/// <binding BeforeBuild='beforeBuild' />
var gulp = require('gulp'),
    gulp_concat = require('gulp-concat'),
    gulp_rename = require('gulp-rename'),
    gulp_watch = require('gulp-watch'),
    gulp_less = require("gulp-less"),
    gulp_uglify = require('gulp-uglify');

gulp.task("less", function () {
    gulp.src('styles/theme*.less')
        .pipe(gulp_less())
        .pipe(gulp.dest('./wwwroot/css'));
});

gulp.task('bootstrap', function () {
    gulp.src('node_modules/bootstrap/dist/js/bootstrap.min.js')
        .pipe(gulp.dest('wwwroot/js'))
    gulp.src('node_modules/bootstrap/dist/css/bootstrap.min.css')
        .pipe(gulp.dest('wwwroot/css'))
})

gulp.task('publish', ['less']);

gulp.task('watch:less', function () {
    gulp.watch('styles/*.less', ['less']);
});

gulp.task('beforeBuild', ['publish']);

gulp.task('watch', ['watch:less']);

gulp.task('default', ['watch', 'bootstrap']);