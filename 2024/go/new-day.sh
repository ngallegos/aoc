#!/bin/sh

day=$1
echo "creating day $day";

mkdir -p ./days/"$day"

touch ./days/"$day"/README.md
touch ./days/"$day"/sample.txt
touch ./days/"$day"/input.txt
sed -e "s/\${day}/$day/" ./new-day-template.txt > ./days/"$day"/day"$day"_test.go