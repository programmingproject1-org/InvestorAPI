#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json
from . import request_config

class ViewDetails():
	def __init__(self, url, token):
		self.url = url
		self.init_header(token)
		self.fetch()

	def init_header(self, token):
		if token is None:
			token = ""
		self.header = {"Content-Type": "application/json", "Authorization": "Bearer " + token}

	def fetch(self):
		self.response = requests.get(self.url, headers = self.header, verify = request_config.VERIFY_HTTPS_REQUEST)

	def get_outcome(self):
		json_details = None

		if self.response.text:
			try:
				json_details = self.response.json()
			except ValueError:
				pass

		return (self.response.status_code, json_details)