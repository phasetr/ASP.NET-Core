#!/bin/bash -xe
# Install OS packages
yum update -y
yum groupinstall -y "Development Tools"
yum install -y gcc openssl11 openssl11-devel bzip2-devel ncurses-devel libffi-devel readline-devel sqlite-devel.x86_64 xz-devel
echo 'export PATH="$HOME/.pyenv/bin:$PATH"' >> ~/.bashrc
echo 'eval "$(pyenv init -)"' >> ~/.bashrc
