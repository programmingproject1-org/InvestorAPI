#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json

class DeletionResponseWrapper():

	SUCCESS_STATUS = 204

	def __init__(self, response):
		self.response = response

	def get_http_status(self):
		return self.response.status_code
