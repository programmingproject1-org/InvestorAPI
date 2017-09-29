#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json
from . import request_config

class CurrentQuotes():
	def __init__(self, url, symbols, token):
		self.url = url
		self.symbols = symbols
		self.init_header(token)
		self.fetch()

	def init_header(self, token):
		if token is None:
			token = ""
		self.header = {"Content-Type": "application/json", "Authorization": "Bearer " + token}

	def fetch(self):
		payload = {"symbols": self.symbols}
		self.response = requests.get(self.url, headers = self.header, params = payload, verify = request_config.VERIFY_HTTPS_REQUEST)

	def get_outcome(self):
		return self.response