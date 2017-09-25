#!/usr/bin/env python
# -*- coding: utf-8 -*-

class ResponseSummary:
	def __init__(self, is_success, error_messages, response_code):
		self.is_success = is_success
		self.error_messages = error_messages
		self.response_code = response_code